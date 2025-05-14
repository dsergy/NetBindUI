namespace NetBindUI.Contracts.Models.Network
{
    /// <summary>
    /// Represents the status of a network interface.
    /// </summary>
    public enum NetworkInterfaceStatus
    {
        /// <summary>
        /// The network interface is up and running.
        /// </summary>
        Up,

        /// <summary>
        /// The network interface is down and not running.
        /// </summary>
        Down,

        /// <summary>
        /// The network interface is in a testing state.
        /// </summary>
        Testing,

        /// <summary>
        /// The network interface status is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The network interface is dormant.
        /// </summary>
        Dormant,

        /// <summary>
        /// The network interface is not present.
        /// </summary>
        NotPresent,

        /// <summary>
        /// The lower layer of the network interface is down.
        /// </summary>
        LowerLayerDown
    }
}