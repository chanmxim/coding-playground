export default function Sidebar({rooms, currentRoom, setRoom, username, onLogout}){
    return (
    <div className="w-64 bg-gray-800 text-white flex flex-col h-full">
        <div className="p-6 border-b border-gray-700">
            <h1 className="text-xl font-bold tracking-wider">ChatApp</h1>
            <p className="text-xs text-gray-400 mt-1">Logged in as {username}</p>
        </div>
        
        <nav className="flex-1 p-4 space-y-2">

        {rooms.map((r) => (
            <button
            key={r}
            onClick={() => setRoom(r)}
            className={`w-full text-left px-4 py-3 rounded transition-colors duration-200 ${
                currentRoom === r 
                ? "bg-green-600 text-white font-semibold shadow-md" 
                : "text-gray-300 hover:bg-gray-800"
            }`}
            >
            {r}
            </button>
        ))}
        </nav>

        <div className="p-4 border-t border-gray-700">
        <button 
            onClick={onLogout} 
            className="w-full py-2 px-4 bg-red-600 hover:bg-red-700 text-white rounded text-sm transition"
        >
            Logout
        </button>
        </div>
    </div>
    )
}