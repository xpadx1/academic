const express = require("express");
const userRoutes = require("./routes/user.routes");
const projectRoutes = require("./routes/project.routes");
const taskRoutes = require("./routes/task.routes");
const collaboratorRoutes = require("./routes/collaborator.routes");
const authRoutes = require("./routes/auth.routes");
const { detectAuth } = require("./middleware/auth.middleware");
const cors = require("cors");

const app = express();

app.use(express.json());
app.use(cors({ origin: "http://localhost:5173" }));
app.use(detectAuth);

app.use("/api/users", userRoutes);
app.use("/api/projects", projectRoutes);
app.use("/api/tasks", taskRoutes);
app.use("/api/collaborators", collaboratorRoutes);
app.use("/api/auth", authRoutes);

app.get("/", (req, res) => {
  res.send("Express + Sequelize + SQLite API");
});

module.exports = app;
