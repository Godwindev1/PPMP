        function toggleGroup(header) {
            const group = header.closest('.task-group');
            const list = group.querySelector('.task-list');
            const icon = header.querySelector('.fa-chevron-down, .fa-chevron-up');
            
            const isActive = group.classList.toggle('active');
            
            // Toggle visibility classes
            if (isActive) {
                list.classList.remove('hidden');
                list.classList.add('block');
                header.classList.remove('bg-slate-50');
                header.classList.add('bg-white');
                icon.classList.replace('fa-chevron-down', 'fa-chevron-up');
            } else {
                list.classList.remove('block');
                list.classList.add('hidden');
                header.classList.add('bg-slate-50');
                header.classList.remove('bg-white');
                icon.classList.replace('fa-chevron-up', 'fa-chevron-down');
            }
        }

        function sendMessage() {
            const input = document.getElementById('chatInput');
            const val = input.value.trim();
            if (val) {
                const box = document.getElementById('chatBox');
                const msgHtml = `
                    <div class="flex flex-col max-w-[90%] ml-auto items-end">
                        <div class="bg-blue-600 text-white text-sm p-3 rounded-lg shadow-sm">
                            ${val}
                        </div>
                    </div>`;
                box.insertAdjacentHTML('beforeend', msgHtml);
                input.value = '';
                box.scrollTop = box.scrollHeight;
            }
        }

        function handleEnter(e) { if(e.key === 'Enter') sendMessage(); }

        // Your custom helper
        if (typeof SetAsActiveSection === "function") {
            SetAsActiveSection("project-view", false);
        }

        
        function OpenAddModal() {
            document.getElementById("taskModal").classList.remove("hidden");
            document.getElementById("taskModal").classList.add("flex");
        }

        function CloseAddModal() {
            document.getElementById("taskModal").classList.add("hidden");
        }

        $('#taskForm').on('submit', function (e) {
            e.preventDefault();
            
            $.ajax({
                url: $(this).attr('action'),
                type: 'POST',
                data: $(this).serialize(),
                success: function (response) {
                    CloseAddModal();
                    // Refresh your client list or show success message
                    location.reload();
                },
                error: function (xhr) {
                    // Handle validation errors
                    console.error('Error:', xhr);
                }
            });
        })

        

