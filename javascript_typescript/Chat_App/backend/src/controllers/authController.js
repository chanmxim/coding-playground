import User from "../models/User.js";
import dotenv from "dotenv";
import bcrypt from "bcrypt";
import jwt from "jsonwebtoken";

dotenv.config();

export const signup = async (req, res) => {
    try{
        const { username, firstname, lastname, password } = req.body;

        const existingUser = await User.findOne({ username });

        if (existingUser){
            return res.status(400).json({
                error: "Username exists"
            })
        }

        const hashedPassword = await bcrypt.hash(password, parseInt(process.env.SALT_ROUNDS));

        const newUser = new User({
            username,
            firstname,
            lastname,
            password: hashedPassword
        });
        await newUser.save();

        res.status(201).json({
            message: "User created"
        })

    } catch(e){
        res.status(500).json({
            error: e.message
        })
    }
}

export const login = async (req, res) => {
    try{
        const {username, password} = req.body;

        const user = await User.findOne({username})

        if (!user){
            return res.status(400).json({
                error: "Username does not exist"
            })
        }

        const isValidPassword = await bcrypt.compare(password, user.password);

        if (!isValidPassword){
            return res.status(400).json({
                error: "Wrong password"
            })
        }

        const token = jwt.sign(
            { id: user._id, username: user.username }, // payload
            process.env.JWT_SECRET, // secret key
            { expiresIn: process.env.JWT_EXPIRES_IN || "1d" } // token expiration
        );

        return res.status(200).json({
            message: "Login successful.",
            token: token,
            username: user.username
        })

    } catch(e){
        res.status(500).json({
            error: e.message
        })
    }
}