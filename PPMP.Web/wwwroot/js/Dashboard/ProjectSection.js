function openProjectModal() {
     console.log("Add-project openProject modal ")
    const content = document.getElementById('ProjectCreateModal');
    content.classList.remove('hidden');
    document.body.style.overflow = 'hidden'; // Prevent scrolling

    content.classList.remove('closing');
}

function closeProjectModal() {
    const content = document.getElementById('ProjectCreateModal');
    document.body.style.overflow = 'auto'; // Re-enable scrolling

    content.classList.add('closing');

    setTimeout(() => {
        content.classList.add('hidden');
        content.classList.remove('closing');
    }, 150);
}


    // Close modal when clicking outside
    const ProjectCreateModal  =  document.getElementById('ProjectCreateModal');
    console.log(`nobviu: ${ProjectCreateModal}`)

    if(ProjectCreateModal != null)
    {
        ProjectCreateModal.addEventListener('click', function (e) {
        if (e.target === this) {
            closeProjectModal();
        }
        });
    }


    // Close modal on Escape key
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            const modal = document.getElementById('ProjectCreateModal');
            if (!modal.classList.contains('hidden')) {
                closeProjectModal();
            }
        }
    });


    $('#createProjectForm').on('submit', function (e) {
        e.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: $(this).serialize(),
            success: function (response) {
                closeProjectModal();
                location.reload();
            },
            error: function (xhr) {
                // Handle validation errors
                console.error('Error:', xhr);
            }
        });
    });


    const AddProjectButton = document.getElementById('add-project')
    console.log(`element: ${AddProjectButton}`)


    AddProjectButton.addEventListener('click', () => {
        console.log("Add-project listener Created")
        openProjectModal();
    });

