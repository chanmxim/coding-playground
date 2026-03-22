import express from "express";
import { body } from "express-validator";

import { getAllEmployees, createEmployee, getEmployeeById, updateEmployeeById, deleteEmployeeById, searchEmployeesByDepartment } from "../controllers/employeeController.js";
import { auth } from "../middleware/authMiddleware.js";
import { upload } from "../middleware/uploadMiddleware.js";


const employeeRouter = express.Router();

employeeRouter.get("/employees", auth, getAllEmployees);

employeeRouter.get("/employees/search", auth, searchEmployeesByDepartment);

employeeRouter.post("/employees", auth,
    upload.single("photo"),
    [
        body("first_name")
            .notEmpty().withMessage("First name is required"),
        body("last_name")
            .notEmpty().withMessage("Last name is required"),
        body("email")
            .notEmpty().withMessage("Email is required")
            .isEmail().withMessage("Invalid email")
            .normalizeEmail(),
        body("position")
            .notEmpty().withMessage("Position is required")
            .trim(),
        body("salary")
            .notEmpty().withMessage("Salary is required")
            .isFloat({ min: 0 }).withMessage("Salary must be a positive number"),
        body("date_of_joining")
            .notEmpty().withMessage("Date of joining is required")
            .isISO8601().toDate().withMessage("Date must be valid"),
        body("department")
            .notEmpty().withMessage("Department is required")
            .trim()
    ],
    createEmployee);

employeeRouter.get("/employees/:eid", auth, getEmployeeById);

employeeRouter.put("/employees/:eid", auth,
    upload.single("photo"),
    [
        body("first_name")
            .optional()
            .notEmpty().withMessage("First name cannot be empty"),
        body("last_name")
            .optional()
            .notEmpty().withMessage("Last name cannot be empty"),
        body("email")
            .optional()
            .notEmpty().withMessage("Email cannot be empty")
            .isEmail().withMessage("Invalid email")
            .normalizeEmail(),
        body("position")
            .optional()
            .notEmpty().withMessage("Position cannot be empty")
            .trim(),
        body("salary")
            .optional()
            .notEmpty().withMessage("Salary cannot be empty")
            .isFloat({ min: 0 }).withMessage("Salary must be a positive number"),
        body("date_of_joining")
            .optional()
            .notEmpty().withMessage("Date of joining cannot be empty")
            .isISO8601().toDate().withMessage("Date must be valid"),
        body("department")
            .optional()
            .notEmpty().withMessage("Department cannot be empty")
            .trim()
    ]
    ,updateEmployeeById);

employeeRouter.delete("/employees/:eid", auth, deleteEmployeeById);

export default employeeRouter;