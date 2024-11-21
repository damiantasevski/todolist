function addTask() {
    $.ajax({
        async: true,
        data: $('#form').serialize(),
        type: "Post",
        url: "/TasksLists/AddTaskToTasksList",
        success: function (partialView) {
            $('#tasks-container').empty();
            $('#tasks-container').html(partialView);
        }
    })
}

function removeTask(taskId) {
    let formData = new FormData(document.getElementById('form'));
    formData.append('taskId', taskId);

    $.ajax({
        async: true,
        data: formData,
        type: "Post",
        url: "/TasksLists/RemoveTaskFromTasksList",
        processData: false,
        contentType: false,
        success: function (partialView) {
            $('#tasks-container').empty();
            $('#tasks-container').html(partialView);
        }
    })
}