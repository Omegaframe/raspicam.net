namespace Raspicam.Net.Native.Util
{
    public enum MmalStatusEnum
    {
        MmalSuccess,
        MmalEnomem,
        MmalEnospc,
        MmalEinval,
        MmalEnosys,
        MmalEnoent,
        MmalEnxio,
        MmalEio,
        MmalEspipe,
        MmalEcorrupt,
        MmalEnotready,
        MmalEconfig,
        MmalEisconn,
        MmalEnotconn,
        MmalEagain,
        MmalEfault,
        MmalStatusMax = 0x7FFFFFFF
    }
}
