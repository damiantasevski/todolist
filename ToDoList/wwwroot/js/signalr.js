function openSignalRConnection() {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notifications")
        .build();

    connection.start()
        .then(() => console.log("SignalR connected"))
        .catch(err => console.error("SignalR connection error:", err));

    connection.on("NewNotification", function (message) {
        console.log("New Notification: " + message);

        const toastElement = `
            <div class="toast show">
            <div class="toast-header">
              <strong class="me-auto">Annoying notification</strong>
              <button type="button" class="btn-close" data-bs-dismiss="toast"></button>
            </div>
            <div class="toast-body">
              <p>${message}</p>
            </div>
          </div>
        `;

        $("#toastContainer").append(toastElement)
    });
}