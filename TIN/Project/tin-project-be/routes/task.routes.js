const express = require("express");
const controller = require("../controllers/task.controller");
const { requireAdmin } = require("../middleware/auth.middleware");

const router = express.Router();

router.post("/", requireAdmin, controller.create);
router.get("/", requireAdmin, controller.findAll);
router.get("/:id", requireAdmin, controller.findById);
router.put("/:id", requireAdmin, controller.update);
router.delete("/:id", requireAdmin, controller.remove);

module.exports = router;
