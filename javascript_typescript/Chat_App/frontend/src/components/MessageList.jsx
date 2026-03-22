export default function MessageList({ messages, username}) {
  if (!messages) {
    return <div className="flex-1 bg-gray-50 flex items-center justify-center text-gray-400">Loading...</div>;
  }

  return (
    <div className="flex-1 overflow-y-auto bg-gray-100">
      {messages.map((msg, i) => {
        const isMe = msg.from_user === username;

        // Format Date
        const time = msg.date_sent 
          ? new Date(msg.date_sent).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) 
          : "";

        return (
          <div key={i} className="w-full bg-white border-b border-gray-200 px-6 py-4 hover:bg-gray-50 transition-colors group">
            <div className="flex justify-between items-start">
              <div className="flex flex-col gap-1">
                {/* Sender Name */}
                <span className={`text-xs font-bold ${isMe ? "text-green-600" : "text-gray-800"}`}>
                  {msg.from_user} {isMe && "(You)"}
                </span>
                
                {/* Message Body */}
                <p className="text-sm text-gray-600 leading-relaxed">
                  {msg.message}
                </p>
              </div>

              {/* Timestamp */}
              <span className="text-[10px] text-gray-400 font-mono whitespace-nowrap ml-4">
                {time}
              </span>
            </div>
          </div>
        );
      })}
    </div>
  );
}