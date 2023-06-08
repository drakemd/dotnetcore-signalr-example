var jwtToken = 'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJPbmxpbmUgSldUIEJ1aWxkZXIiLCJpYXQiOjE2ODYxMjczODQsImV4cCI6MTcxNzY2MzM4NCwiYXVkIjoid3d3LmV4YW1wbGUuY29tIiwic3ViIjoianJvY2tldEBleGFtcGxlLmNvbSIsIkdpdmVuTmFtZSI6IkpvaG5ueSIsIlN1cm5hbWUiOiJSb2NrZXQiLCJFbWFpbCI6ImNvb2tAY2hhdGltZS5jb20iLCJSb2xlIjpbIkNvb2siLCJNYW5hZ2VyIl19.Se0Ef1Pi0PR5F4IFpCZbZeSuFIg_yJYgO84JYfBhYXI';
var connection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:7042/orderHub', { accessTokenFactory: () => jwtToken })
    .build();
var orders = [];

connection.on("ReceiveOrder", async function (order) {
    if(orders.length === 0 || (order.id - orders[orders.length - 1].id === 1)) {
        orders.push(order);
        generateOrderList();
    }else {
        var from = orders[orders.length - 1].id + 1;
        var to = order.id - 1;
        var data = await getUnavailableOrders(from, to);
        data.forEach(d => {
            orders.push(d);
        })
        orders.push(order);
        generateOrderList();
    }
});

async function connectSignalR() {
    try {
        await connection.start()
    } catch (err) {
        console.error(err.toString());
    }
}

async function disconnectSignalR() {
    try {
        await connection.stop()
    } catch (err) {
        console.error(err.toString());
    }
}

async function getUnavailableOrders(from, to) {
    var data = await fetch(`https://localhost:7042/order?from=${from}&to=${to}`);
    return data.json();
}

// Function to generate the order list
function generateOrderList() {
    const orderList = document.getElementById('orderList');
    orderList.innerHTML = '';

    orders.forEach((order) => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${order.id}</td>
            <td>${order.details}</td>
            <td>${order.status}</td>
        `;
        orderList.appendChild(row);
    });
}

connectSignalR();