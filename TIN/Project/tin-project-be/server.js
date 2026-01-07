const app = require("./app");
const { sequelize } = require("./models");

const PORT = 3000;

(async () => {
  try {
    await sequelize.authenticate();
    console.log("Database connected");

    await sequelize.sync(); // use { force: true } to reset DB
    console.log("Models synchronized");

    app.listen(PORT, () => {
      console.log(`Server running at http://localhost:${PORT}`);
    });
  } catch (err) {
    console.error("Failed to start server:", err);
  }
})();
