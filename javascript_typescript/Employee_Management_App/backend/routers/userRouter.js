import express from "express";
import { body } from "express-validator";

import { createUser, login } from "../controllers/userController.js";

const userRouter = express.Router();

userRouter.post("/signup", 
    [
        body("username")
            .notEmpty().withMessage("Username is required")
            .isLength({ min: 3 }).withMessage("Username must be at least 3 characters")
            .trim(),
        body("email")
            .notEmpty().withMessage("Email is required")
            .isEmail().withMessage("Invalid email")
            .normalizeEmail(),
        body("password")
            .notEmpty().withMessage("Password is required")
    ],
    createUser);

userRouter.post("/login", 
    [
        body("username")
            .optional()
            .notEmpty().withMessage("Username is required")
            .isLength({ min: 3 }).withMessage("Username must be at least 3 characters")
            .trim(),
        body("email")
            .optional()
            .notEmpty().withMessage("Email is required")
            .isEmail().withMessage("Invalid email")
            .normalizeEmail(),
        // At least username or email must exist
        body().custom(body => {
            if (!body.username && !body.email) {
                throw new Error("Username or email is required");
            }
            return true;
        }),
        body("password")
            .notEmpty().withMessage("Password is required")
    ],
    login);

export default userRouter;