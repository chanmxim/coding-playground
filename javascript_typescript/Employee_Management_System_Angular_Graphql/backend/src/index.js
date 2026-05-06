import dotenv from "dotenv";
import express from "express"
import { ApolloServer } from '@apollo/server';
import { expressMiddleware } from '@as-integrations/express5';
import cors from "cors";
import jwt from 'jsonwebtoken';

import typeDefs from "./graphql/schema/schema.js";
import resolvers from "./graphql/resolvers/resolvers.js";
import { connectDb } from "./config/db.js";

dotenv.config({ override: true })

const PORT = process.env.PORT

const app = express()

const server = new ApolloServer({
    typeDefs,
    resolvers
})

await server.start()

app.use(
    '/graphql',
    cors(),
    express.json({ limit: '50mb' }),
    expressMiddleware(server, {
        context: async ({ req }) => {
            const authHeader = req.headers.authorization || '';
            const token = authHeader.startsWith('Bearer ') 
                ? authHeader.split(' ')[1] 
                : null;

            if (!token) return { user: null };

            try {
                const decoded = jwt.verify(token, process.env.JWT_SECRET);
                return { user: decoded };
            } catch (err) {
                return { user: null };
            }
        },
    })
)

try{
    await connectDb()
    app.listen(PORT, () => {
        console.log("Server is running on port " + PORT);
    })
} catch(e){
    console.error("Failed to start server:", err);
}