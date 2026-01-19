function openClientModal() {
    const content = document.getElementById('ClientFormModal');
    content.classList.remove('hidden');
    document.body.style.overflow = 'hidden'; // Prevent scrolling

    content.classList.remove('closing');
}

function closeClientModal() {
    const content = document.getElementById('ClientFormModal');
    document.body.style.overflow = 'auto'; // Re-enable scrolling

    content.classList.add('closing');

    setTimeout(() => {
        content.classList.add('hidden');
        content.classList.remove('closing');
    }, 250);
}


// Close modal when clicking outside
document.getElementById('ClientFormModal').addEventListener('click', function (e) {
    if (e.target === this) {
        closeClientModal();
    }
});

// Close modal on Escape key
document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape') {
        const modal = document.getElementById('ClientFormModal');
        if (!modal.classList.contains('hidden')) {
            closeClientModal();
        }
    }
});


//Developer Dashboard page scripting
    $('#createClientForm').on('submit', function (e) {
        e.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: $(this).serialize(),
            success: function (response) {
                closeClientModal();
                // Refresh your client list or show success message
                location.reload();
            },
            error: function (xhr) {
                // Handle validation errors
                console.error('Error:', xhr);
            }
        });
    });


    const AddButton = document.getElementById('Add-client')
    
    AddButton.addEventListener('click', () => {
        console.log("Add-client listener")
        openClientModal();
    });
