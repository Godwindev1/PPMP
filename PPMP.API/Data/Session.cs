using PPMP.Data;

public class SessionPage
{
    public Guid SessionID {get; set;}
    public Guid CLientID {get;set;}
    public string DeveloperID {get;set;}

    //NAVIGATION 
    public User? Developer {get; set; }
    public Client? client {get; set; }
}