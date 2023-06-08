var jwtToken = 'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJPbmxpbmUgSldUIEJ1aWxkZXIiLCJpYXQiOjE2ODYxMjczODQsImV4cCI6MTcxNzY2MzM4NCwiYXVkIjoid3d3LmV4YW1wbGUuY29tIiwic3ViIjoianJvY2tldEBleGFtcGxlLmNvbSIsIkdpdmVuTmFtZSI6Ik1hZCIsIlN1cm5hbWUiOiJNYXgiLCJFbWFpbCI6IndhaXRlckBjaGF0aW1lLmNvbSIsIlJvbGUiOiJXYWl0ZXIifQ.fRRjMogMMnM5e15fX4oUZZG11rPt5E0p5aEbpsK5XCQ';
var connection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:7042/orderHub', { accessTokenFactory: () => jwtToken })
    .build();

async function connectSignalR() {
    try {
        await connection.start();
    } catch (err) {
        console.error(err.toString());
    }
}

async function disconnectSignalR() {
    try {
        await connection.stop();
    } catch (err) {
        console.error(err.toString());
    }
}

// Get the order form
const orderForm = document.getElementById('orderForm');

// Add event listener to the order form submission
orderForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    
    // Get the entered order details
    const orderDetails = document.getElementById('orderDetails').value;
    
    // Process the order details (you can add your own logic here)
    await fetch('https://localhost:7042/order', {
        headers: {
            'Content-Type': 'application/json'
        },
        method: 'POST',
        body: JSON.stringify({
            details: orderDetails
        })
    });

    // Clear the form inputs
    document.getElementById('orderDetails').value = '';
    
    // Display a success message (you can modify this as per your requirements)
    alert('Order added successfully!');
});

connectSignalR();