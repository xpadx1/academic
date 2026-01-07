"use strict";

module.exports = {
  async up(queryInterface) {
    await queryInterface.bulkInsert("projects", [
      {
        name: "Website Redesign",
        budget: 50000,
        description: "New company website",
        startDate: new Date(),
        endDate: null,
        createdAt: new Date(),
        updatedAt: new Date(),
      },
    ]);
  },

  async down(queryInterface) {
    await queryInterface.bulkDelete("projects", null, {});
  },
};
