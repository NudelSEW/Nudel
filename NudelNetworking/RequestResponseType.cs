using System;
using System.Collections.Generic;
using System.Text;

namespace Nudel.Networking
{
    public enum RequestResponseType
    {
        Register,
        Login,
        Logout,
        CreateEvent,
        EditEvent,
        DeleteEvent,
        FindEvent,
        InviteToEvent,
        AcceptEvent,
        LeaveEvent,
        AddComment,
        DeleteComment,
        FindCurrentUser,
        FindUser,
        EditUser,
        DeleteUser,
    }
}
