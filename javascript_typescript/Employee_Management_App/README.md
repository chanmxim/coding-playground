# Employee Management App

A full-stack Employee Management web application built with **React**, **Express.js**, **MongoDB Atlas**, and **Redux**. Users can add, update, delete, and search employees. Authentication is implemented with JWT.

---

## Instructions to Run Locally

1. **Clone the repository**  

2. **Navigate to backend/frontend folder**
```bash
cd backend
cd frontend
```

3. **Start the application**
```bash
npm run dev
```
---

**Troubleshooting**  
- If there are any errors, they are most likely related to the database connection.  
- If the app cannot connect to the provided MongoDB Atlas, replace the connection string in `.env` with your own MongoDB Atlas URI.  
- Ensure that a database named `comp3123_assignment1` exists in your MongoDB cluster.