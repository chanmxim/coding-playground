import Employee from "../models/employee.js";
import { validationResult } from "express-validator";
import mongoose from "mongoose";

export const getAllEmployees = async (req, res) => {
    try{
        // Find all employees
        const employees = await Employee.find();

        // Format response data
        const response = employees.map(employee => ({
            employee_id: employee._id,
            first_name: employee.first_name,
            last_name: employee.last_name,
            email: employee.email,
            position: employee.position,
            salary: employee.salary,
            date_of_joining: employee.date_of_joining,
            department: employee.department
        }))
        
        return res.status(200).json({
            status: true,
            data: response
        })
        
    } catch (err){
        console.log("Error: ", err);
        return res.status(500).json({
            status: false,
            message: "Internal server error"
        })
    }
}

export const searchEmployeesByDepartment = async (req, res) => {
    try {
        const { department } = req.query;
    
        if (!department) {
            return res.status(400).json({
                status: false,
                message: "Department query parameter is required"
            });    
        }

        const employees = await Employee.find({ department: department.trim() });

        return res.status(200).json({
            status: true,
            data: employees
        });

    } catch (err){
        console.log("Error: ", err);
        return res.status(500).json({
            status: false,
            message: "Internal server error"
        })
    }
}

export const createEmployee = async (req, res) => {
    try{
        // Validate errors
        const errors = validationResult(req);

        if (!errors.isEmpty()){
            return res.status(400).json({ 
                status: false,
                message: "Invalid user entry",
                errors: errors.array() 
            });
        }

        const { 
            first_name, 
            last_name,
            email, 
            position, 
            salary, 
            date_of_joining, 
            department 
        } = req.body;
        
        // Check if employee with such email exists
        const existingEmail = await Employee.findOne({ email });

        if (existingEmail){
            return res.status(400).json({
                status: false,
                message: "Employee with this email already exists"
            })
        }

        // Create employee
        const employee = new Employee({
            first_name,
            last_name,
            email,
            position,
            salary,
            date_of_joining,
            department,
            photo: req.file?.buffer,
            photoType: req.file?.mimetype,
        });

        await employee.save();

        return res.status(201).json({
            status: true,
            message: "Employee created successfully.",
            employee_id: employee._id
        })
    
    } catch (err){
        console.log("Error: ", err);
        return res.status(500).json({
            status: false,
            message: "Internal server error"
        })
    }
    

}

export const getEmployeeById = async (req, res) => {
    try{
        const { eid } = req.params;

        // Validate id
        if (!mongoose.Types.ObjectId.isValid(eid)) {
                return res.status(400).json({
                    status: false,
                    message: "Invalid employee ID."
            })
        }
        
        // Find employee by id
        const employee = await Employee.findById(eid);

        if (!employee){
            return res.status(400).json({ 
                status: false,
                message: "Employee not found",
            });
        }

        // Format response data
        const response = {
            employee_id: employee._id,
            first_name: employee.first_name,
            last_name: employee.last_name,
            email: employee.email,
            position: employee.position,
            salary: employee.salary,
            date_of_joining: employee.date_of_joining,
            department: employee.department,
            photo: employee.photo?.toString("base64"),
            photoType: employee.photoType
        }

        return res.status(200).json({
            status: true,
            data: response
        })
        
    } catch (err){
        console.log("Error: ", err);
        return res.status(500).json({
            status: false,
            message: "Internal server error"
        })
    }
}

export const updateEmployeeById = async (req, res) => {
    try{
        const { eid } = req.params;

        // Validate id
        if (!mongoose.Types.ObjectId.isValid(eid)) {
                return res.status(400).json({
                    status: false,
                    message: "Invalid employee ID."
                })
        }

        // Validate errors
        const errors = validationResult(req);

        if (!errors.isEmpty()){
            return res.status(400).json({ 
                status: false,
                message: "Invalid user entry",
                errors: errors.array() 
            });
        }

        const dataToUpdate = req.body;

        if (req.file) {
            dataToUpdate.photo = req.file.buffer;
            dataToUpdate.photoType = req.file.mimetype;
        }

        // Update employee
        const updatedEmployee = await Employee.findByIdAndUpdate(eid, dataToUpdate, { new: true });

        if (!updatedEmployee){
            return res.status(400).json({ 
                status: false,
                message: "Employee not found",
            });
        }

        return res.status(200).json({ 
            status: true,
            message: "Employee details updated successfully."
        });


    } catch (err){
        console.log("Error: ", err);
        return res.status(500).json({
            status: false,
            message: "Internal server error"
        })
    }

}

export const deleteEmployeeById = async (req, res) => {
    try {
        const { eid } = req.params;
        
        // Validate id
        if (!mongoose.Types.ObjectId.isValid(eid)) {
            return res.status(400).json({
                status: false,
                message: "Invalid or missing employee ID."
            })
        }

        // Delete employee
        const deletedEmployee = await Employee.findByIdAndDelete(eid);

        if (!deletedEmployee){
            return res.status(400).json({ 
                status: false,
                message: "Employee not found",
            });
        }

        return res.status(204).send();

    } catch (err){
        console.log("Error: ", err);
        return res.status(500).json({
            status: false,
            message: "Internal server error"
        })
    }

}