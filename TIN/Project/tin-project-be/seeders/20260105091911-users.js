"use strict";
const bcrypt = require("bcrypt");
module.exports = {
  async up(queryInterface) {
    await queryInterface.bulkInsert("users", [
      {
        name: "Alice",
        email: "alice@test.com",
        isAdmin: true,
        department: "Engineering",
        password: await bcrypt.hash("Qwerty123", 10),
        createdAt: new Date(),
        updatedAt: new Date(),
      },
      {
        name: "Bob",
        email: "bob@test.com",
        isAdmin: false,
        department: "Marketing",
        password: await bcrypt.hash("Qwerty123", 10),
        createdAt: new Date(),
        updatedAt: new Date(),
      },
    ]);
  },

  async down(queryInterface) {
    await queryInterface.bulkDelete("users", null, {});
  },
};
