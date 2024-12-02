const express = require('express');
const dotenv = require('dotenv');

// Initialize dotenv to use environment variables
dotenv.config();

const app = express();
const port = process.env.PORT || 5000; // Default to 5000 if no port is specified

// Middleware to parse incoming JSON requests
app.use(express.json());

// Simple route to test server is working
app.get('/', (req, res) => {
    res.send('Hello, World!');
});

// Example API route (you can add more as needed)
app.get('/api', (req, res) => {
    res.json({ message: 'Hello from the API!' });
});

// Start server
app.listen(port, () => {
    console.log(`Server is running on http://localhost:${port}`);
});
