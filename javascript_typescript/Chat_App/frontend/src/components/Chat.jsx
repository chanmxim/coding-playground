import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import io from "socket.io-client";
import axios from "axios";
import Sidebar from "./Sidebar";
import MessageList from "./MessageList";

const socket_url = "http://localhost:3000";
const rooms = ["room1", "room2", "room3", "room4"];

export default function Chat() {
  const navigate = useNavigate();
  const [socket, setSocket] = useState(null);
  const [room, setRoom] = useState(rooms[0]);
  const [messages, setMessages] = useState([]);
  const [inputMessage, setInputMessage] = useState("");
  
  const username = localStorage.getItem("username");
  const token = localStorage.getItem("token");

  // Initial connection
  useEffect(() => {
    if (!username && !token) {
      navigate("/login");
      return;
    }

    const newSocket = io(socket_url);
    setSocket(newSocket);

    newSocket.on("message", (msg) => {
      setMessages((prev) => [...prev, msg]);
    });

    return () => newSocket.disconnect();
  }, []);

  // Switching rooms
  useEffect(() => {
    if (!socket) return;

    setMessages([]);

    const fetchHistory = async () => {
      try {
        const res = await axios.get(`${socket_url}/api/chat/${room}`);

        if (res.data) {
            setMessages(res.data.messages);
        }
      } catch (err) {
        console.error("Failed to load history:", err);
      }
    };

    fetchHistory();
    
    socket.emit("joinedRoom", { username, room });

    return () => {
      socket.emit("leftRoom", { username, room });
    };
  }, [room, socket, username]);

  
  const sendMessage = (e) => {
    e.preventDefault();

    if (inputMessage.trim() && socket) {
      socket.emit("groupChatMessage", {
        from_user: username,
        room,
        message: inputMessage
      });
      setInputMessage("");
    }
  };

  const logout = () => {
    localStorage.clear();
    navigate("/login");
  };

  return (
    <div className="flex h-screen bg-gray-100 font-sans">
      <Sidebar 
        rooms={rooms} 
        currentRoom={room} 
        setRoom={setRoom} 
        username={username} 
        onLogout={logout} 
      />

      <div className="flex-1 flex flex-col min-w-0">
        {/* Header */}
        <header className="bg-white border-b border-gray-200 h-16 flex items-center px-6 shadow-sm z-10">
          <span className="text-xl font-bold text-gray-800 tracking-tight">
            {room}
          </span>
        </header>

        {/* Messages */}
        <MessageList 
          messages={messages} 
          username={username} 
        />

        {/* Input Area */}
        <div className="bg-white border-t border-gray-200 p-4">
          <form onSubmit={sendMessage} className="flex gap-4 max-w-5xl mx-auto">
            <input
              type="text"
              value={inputMessage}
              onChange={(e) => setInputMessage(e.target.value)}
              placeholder={`Message #${room}...`}
              className="flex-1 bg-gray-100 text-gray-800 border-none rounded-lg px-4 py-3 focus:bg-white"
            />
            <button 
              type="submit" 
              className="bg-green-600 text-white font-medium px-8 py-3 rounded-lg transition-colors shadow-sm"
            >
              Send
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}