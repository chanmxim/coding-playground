import mongoose from "mongoose";
import User from "../models/user.js";
import bcrypt from "bcrypt";
import dotenv from "dotenv";
import { validationResult } from "express-validator";
import jwt from "jsonwebtoken";

dotenv.config();

export const createUser = async (req, res) => {
    try{
        // Validate errors
        const errors = validationResult(req);

        if (!errors.isEmpty()){
            return res.status(400).json({ 
                status: false,
                message: "Invalid credentials",
                errors: errors.array() 
            });
        }

        const { username, email, password } = req.body;

        // Check if user is registered
        const registeredUser = await User.findOne({
            $or: [{email}, {username}]
        })

        if (registeredUser){
            return res.status(400).json({
                status: false,
                message: "Username or email already exists"
            })
        }

        // Hash password
        const hashedPassword = await bcrypt.hash(password, parseInt(process.env.SALT_ROUNDS));

        // Create user
        const user = new User({
            username,
            email,
            password: hashedPassword
        })

        await user.save();

        return res.status(201).json({
            status: true,
            message: "User created successfully.",
            user_id: user._id
        })
    } catch (err){
        console.log("Error: ", err);
        return res.status(500).json({
            status: false,
            message: "Internal server error"
        })
    }

}

export const login = async (req, res) => {
    try {
        // Validate errors
        const errors = validationResult(req);

        if (!errors.isEmpty()){
            return res.status(400).json({ 
                status: false,
                message: "Invalid credentials",
                errors: errors.array() 
            });
        }

        const { username, email, password } = req.body;

        // Check what loginId user sent - either email or username
        let condition = {};

        if (email) condition = {email};
        if (username) condition = {username};

        // Find user
        const user = await User.findOne(condition);

        if (!user) {
            return res.status(400).json({
                status: false,
                message: "Invalid credentials (username/email)"
            })
        }

        // Compare passwords
        const isValidPassword = await bcrypt.compare(password, user.password);

        if (!isValidPassword){
            return res.status(400).json({
                status: false,
                message: "Invalid credentials (password)"
            })
        }
        
        const token = jwt.sign(
            { id: user._id, username: user.username }, // payload
            process.env.JWT_SECRET, // secret key
            { expiresIn: process.env.JWT_EXPIRES_IN || "1d" } // token expiration
        );

        return res.status(200).json({
            status: true,
            message: "Login successful.",
            token: token

        })


    } catch (err){
        console.log("Error: ", err);
        return res.status(500).json({
            status: false,
            message: "Internal server error"
        })
    }
}