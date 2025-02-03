public class Helper {
    static private int MIN_MTU = 576;
    static private int MAX_IP_HEADER_SIZE = 60;
    static private int UDP_HEADER_SIZE = 8;
    static public int MAX_DATAGRAM_SIZE = MIN_MTU - MAX_IP_HEADER_SIZE - UDP_HEADER_SIZE;
    public static final String[] operations = {"ADD", "SUB", "MUL", "DIV"};
    public static int[] statistics = {0, 0, 0, 0}; // {new devices, computed requests, incorrect operations, sum of values}
    public static int[] requestedOperation = {0, 0, 0, 0}; // ADD, SUB, MUL, DIV
}
