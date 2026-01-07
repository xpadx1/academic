const express = require("express");
const controller = require("../controllers/project.controller");
const { requireAuth } = require("../middleware/auth.middleware");

const router = express.Router();

router.post("/", requireAuth, controller.create);
router.get("/", controller.findAll);
router.get("/:id", controller.findById);
router.put("/:id", requireAuth, controller.update);
router.delete("/:id", requireAuth, controller.remove);

module.exports = router;
