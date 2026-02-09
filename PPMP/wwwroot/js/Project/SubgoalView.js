
function updateAddTaskPosition(taskList) {
    const addRow = taskList.querySelector('.add-task-row');
    const tasks = taskList.querySelectorAll('.task-item'); // class used in _Task
}


function submitTask(e, input) {
    if (e.key !== 'Enter') return;

    e.preventDefault();

    const value = input.value.trim();
    if (!value) return;

    const form = input.closest('form');
    const taskList = input.closest('.task-list');

    $.ajax({
        url: form.action,
        type: 'POST',
        data: $(form).serialize(),
        success: function (html) {
            // Server should return rendered _Task partial
            taskList.insertAdjacentHTML('beforeend', html);
            input.value = '';
            updateAddTaskPosition(taskList);
        },
        error: function (xhr) {
            console.error('Error:', xhr);
        }
    });
}
