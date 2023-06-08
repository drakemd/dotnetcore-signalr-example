// Get the login form
const loginForm = document.getElementById('loginForm');

// Add event listener to the login form submission
loginForm.addEventListener('submit', (e) => {
    e.preventDefault();
    
    // Get the entered username
    const username = document.getElementById('username').value;
    
    // Redirect based on the username
    if (username.toLowerCase() === 'cook') {
        window.location.href = 'kitchen.html';
    } else if (username.toLowerCase() === 'waiter') {
        window.location.href = 'waiter.html';
    }
});