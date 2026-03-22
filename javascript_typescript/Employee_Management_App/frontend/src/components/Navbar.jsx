import { useSelector, useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { logout } from "../redux/slices/authSlice";
import { Button } from "../components/ui/button";

export function Navbar() {
  const { isAuthenticated } = useSelector((state) => state.auth);
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const handleLogout = () => {
    dispatch(logout());
    navigate("/login");
  };

  return (
    <nav className="bg-black p-4 text-white flex justify-between items-center shadow-md">
      <h2
        className="text-xl font-bold cursor-pointer"
        onClick={() => navigate("/dashboard")}
      >
        Employee Management
      </h2>

      <div className="flex items-center gap-3">
        {!isAuthenticated && (
          <>
            <Button
              onClick={() => navigate("/login")}
              className="bg-white text-black hover:bg-gray-200"
            >
              Login
            </Button>
            <Button
              onClick={() => navigate("/signup")}
              className="bg-white text-black hover:bg-gray-200"
            >
              Signup
            </Button>
          </>
        )}

        {isAuthenticated && (
          <>
            <Button
              onClick={() => navigate("/dashboard")}
              className="bg-white text-black hover:bg-gray-200"
            >
              Dashboard
            </Button>
            <Button
              onClick={handleLogout}
              className="bg-white text-red-600 hover:bg-gray-200"
            >
              Logout
            </Button>
          </>
        )}
      </div>
    </nav>
  );
}
