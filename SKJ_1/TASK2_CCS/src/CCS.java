import java.io.IOException;
import java.net.*;
import java.util.Objects;

public class CCS {
    static private int port;

    public static void main(String[] args) throws SocketException {
        if (args.length != 1) {
            System.out.println("Usage: java -jar CCS.jar <port>");
            return;
        }

        try {
            port = Integer.parseInt(args[0]);
        }
        catch (Exception e){
            System.out.println("Provide a valid integer for <port>");
            return;
        }

        try {
            Thread tcpThread = new Thread(() -> {
                try {
                    TCPServer tcp = new TCPServer(port);
                    tcp.listenSocket();
                } catch (Exception e) {
                    System.err.println("Error in TCP server: " + e.getMessage());
                }
            });

            Thread udpThread = new Thread(() -> {
                try {
                    UDPServer udp = new UDPServer(port);
                    udp.listen();
                } catch (Exception e) {
                    System.err.println("Error in UDP server: " + e.getMessage());
                }
            });

            tcpThread.start();
            udpThread.start();
        }
        catch(Exception e) {
            System.err.println("Error: " + e.getMessage());
        }
        boolean skipFirst = true;
        while (true) {
            if (skipFirst) {
                skipFirst = false;
            }
            else {
                System.out.println("Statistics");
                System.out.println("New devices connected: " + Helper.statistics[0]);
                System.out.println("Computed requests: " + Helper.statistics[1]);
                System.out.println("Incorrect operations received: " + Helper.statistics[2]);
                System.out.println("General sum of all received values: " + Helper.statistics[3]);
                System.out.println("Requested operations");
                System.out.println("ADD: " + Helper.requestedOperation[0]);
                System.out.println("SUB: " + Helper.requestedOperation[1]);
                System.out.println("MUL: " + Helper.requestedOperation[2]);
                System.out.println("DIV: " + Helper.requestedOperation[3]);
            }
            try {
                Thread.sleep(10000);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }
}
