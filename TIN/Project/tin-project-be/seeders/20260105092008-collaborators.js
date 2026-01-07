"use strict";

module.exports = {
  async up(queryInterface) {
    await queryInterface.bulkInsert("collaborators", [
      {
        user_id: 1,
        project_id: 1,
        role: "manager",
        hours: 0,
        isActive: true,
        createdAt: new Date(),
        updatedAt: new Date(),
      },
    ]);
  },

  async down(queryInterface) {
    await queryInterface.bulkDelete("collaborators", null, {});
  },
};
