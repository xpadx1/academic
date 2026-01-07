"use strict";

module.exports = {
  async up(queryInterface) {
    await queryInterface.bulkInsert("tasks", [
      {
        user_id: 1,
        project_id: 1,
        name: "Design homepage mockup",
        description: "Create initial design mockups for the new homepage",
        status: "in_progress",
        priority: "high",
        hours: 8,
        createdAt: new Date(),
        updatedAt: new Date(),
      },
    ]);
  },

  async down(queryInterface) {
    await queryInterface.bulkDelete("tasks", null, {});
  },
};
