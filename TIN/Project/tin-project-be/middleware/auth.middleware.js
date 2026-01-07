const jwt = require("jsonwebtoken");
const { User } = require("../models");

const JWT_SECRET = "supersecret";

exports.detectAuth = async (req, res, next) => {
  req.auth = {
    isGuest: true,
    isUser: false,
    isAdmin: false,
    user: null,
  };

  const header = req.headers.authorization;

  if (!header || !header.startsWith("Bearer ")) {
    return next();
  }

  try {
    const token = header.split(" ")[1];
    const decoded = jwt.verify(token, JWT_SECRET);

    const user = await User.findByPk(decoded.id, {
      attributes: { exclude: ["password"] },
    });

    if (!user) return next();

    req.auth = {
      isGuest: false,
      isUser: true,
      isAdmin: user.isAdmin === true,
      user,
    };

    next();
  } catch {
    next();
  }
};

exports.requireAuth = (req, res, next) => {
  if (req.auth?.isGuest) {
    return res.status(401).json({ message: "Authentication required" });
  }
  next();
};

exports.requireAdmin = (req, res, next) => {
  if (!req.auth?.isAdmin) {
    return res.status(403).json({ message: "Admin access required" });
  }
  next();
};
