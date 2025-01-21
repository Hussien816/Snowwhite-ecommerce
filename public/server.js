const express = require('express');
const path = require('path');
const admin = require('firebase-admin');
const app = express();

// Firebase Admin SDK initialization
const serviceAccount = require('./path/to/serviceAccountKey.json'); // Update with the path to your service account key

admin.initializeApp({
    credential: admin.credential.cert(serviceAccount)
});


app.use(express.static(__dirname)); // Serve static files


// Handle all routes by serving index.html
app.get('*', (req, res) => {
    res.sendFile(path.join(__dirname, req.path));
});

const PORT = 5000; // Updated port
app.listen(PORT, () => {
    console.log(`Server running at http://localhost:${PORT}`);
});
