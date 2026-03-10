using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs;

public class ChatHub : Hub
{
    // In-memory store for connected users
    private static readonly Dictionary<string, string> ConnectedUsers = new();
    private static readonly List<object> MessageHistory = new();

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (ConnectedUsers.TryGetValue(Context.ConnectionId, out var username))
        {
            ConnectedUsers.Remove(Context.ConnectionId);
            await Clients.All.SendAsync("UserLeft", username, ConnectedUsers.Values.ToList());
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinChat(string username)
    {
        // Check if username already taken
        if (ConnectedUsers.ContainsValue(username))
        {
            await Clients.Caller.SendAsync("UsernameTaken");
            return;
        }

        ConnectedUsers[Context.ConnectionId] = username;

        // Send message history to new user
        await Clients.Caller.SendAsync("LoadHistory", MessageHistory);

        // Notify everyone
        await Clients.All.SendAsync("UserJoined", username, ConnectedUsers.Values.ToList());

        var systemMsg = new
        {
            type = "system",
            text = $"{username} joined the chat",
            timestamp = DateTime.UtcNow.ToString("HH:mm")
        };
        await Clients.All.SendAsync("ReceiveSystemMessage", systemMsg);
    }

    public async Task SendMessage(string message)
    {
        if (!ConnectedUsers.TryGetValue(Context.ConnectionId, out var username))
            return;

        var msg = new
        {
            type = "message",
            username,
            text = message,
            timestamp = DateTime.UtcNow.ToString("HH:mm"),
            connectionId = Context.ConnectionId
        };

        // Save to history (keep last 100 messages)
        MessageHistory.Add(msg);
        if (MessageHistory.Count > 100)
            MessageHistory.RemoveAt(0);

        await Clients.All.SendAsync("ReceiveMessage", msg);
    }

    public async Task SendPrivateMessage(string targetUsername, string message)
    {
        if (!ConnectedUsers.TryGetValue(Context.ConnectionId, out var senderUsername))
            return;

        var targetConnectionId = ConnectedUsers.FirstOrDefault(u => u.Value == targetUsername).Key;
        if (targetConnectionId == null)
        {
            await Clients.Caller.SendAsync("ErrorMessage", $"User '{targetUsername}' not found.");
            return;
        }

        var msg = new
        {
            type = "private",
            from = senderUsername,
            to = targetUsername,
            text = message,
            timestamp = DateTime.UtcNow.ToString("HH:mm")
        };

        await Clients.Client(targetConnectionId).SendAsync("ReceivePrivateMessage", msg);
        await Clients.Caller.SendAsync("ReceivePrivateMessage", msg);
    }

    public async Task SendTyping()
    {
        if (!ConnectedUsers.TryGetValue(Context.ConnectionId, out var username))
            return;

        await Clients.Others.SendAsync("UserTyping", username);
    }

    public async Task StopTyping()
    {
        if (!ConnectedUsers.TryGetValue(Context.ConnectionId, out var username))
            return;

        await Clients.Others.SendAsync("UserStoppedTyping", username);
    }
}