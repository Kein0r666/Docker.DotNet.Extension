using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.DotNet.Extension.Models.Enums
{
    public enum State
    {
        None = 0,
        Created = 1,
        Restarting = 2,
        Running = 3,
        Removing = 4,
        Paused = 5,
        Exited = 6,
        Dead = 7,
    }

    public static class ContainerStateExtension
    {
        public static string ToName(this State state)
        {
            return state.ToString("G").ToLower();
        }

        public static State ToState(this string state)
        {
            if (string.IsNullOrWhiteSpace(state))
                throw new ArgumentNullException(state);

            if (state.Equals(State.Created.ToString("G"), StringComparison.InvariantCultureIgnoreCase))
                return State.Created;
            if (state.Equals(State.Restarting.ToString("G"), StringComparison.InvariantCultureIgnoreCase))
                return State.Restarting;
            if (state.Equals(State.Running.ToString("G"), StringComparison.InvariantCultureIgnoreCase))
                return State.Running;
            if (state.Equals(State.Removing.ToString("G"), StringComparison.InvariantCultureIgnoreCase))
                return State.Removing;
            if (state.Equals(State.Paused.ToString("G"), StringComparison.InvariantCultureIgnoreCase))
                return State.Paused;
            if (state.Equals(State.Exited.ToString("G"), StringComparison.InvariantCultureIgnoreCase))
                return State.Exited;
            if (state.Equals(State.Dead.ToString("G"), StringComparison.InvariantCultureIgnoreCase))
                return State.Dead;

            return State.None;
        }

        public static bool Compare(this State status, string statusName)
        {
            return status == statusName.ToState();
        }
    }
}
