const { Sequelize } = require("sequelize");

const sequelize = new Sequelize({
  dialect: "sqlite",
  storage: "./database.sqlite",
  logging: false,
  dialectOptions: {
    foreign_keys: true,
  },
});

module.exports = sequelize;
