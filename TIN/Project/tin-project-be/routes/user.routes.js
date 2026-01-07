const express = require("express");
const controller = require("../controllers/user.controller");
const { requireAdmin } = require("../middleware/auth.middleware");

const router = express.Router();

router.post("/", requireAdmin, controller.createUser);
router.get("/", requireAdmin, controller.getUsers);
router.get("/:id", requireAdmin, requireAdmin, controller.getUserById);
router.put("/:id", requireAdmin, controller.updateUser);
router.delete("/:id", requireAdmin, controller.deleteUser);

module.exports = router;
