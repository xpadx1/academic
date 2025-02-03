import java.util.ArrayList;
import java.util.Arrays;

public class Helper {
    public static int getRandomPort(int length) {
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < length; i++) {
            if (i == 0) {
                result.append((int)(Math.random() * 5) + 1);
                continue;
            }
            result.append((int)(Math.random() * 9));
        }

        return Integer.parseInt(result.toString());
    }

    public static int average(ArrayList<Integer> arr){
        int sum = 0;
        for (int num : arr) {
            if (num != 0) {
                sum += num;
            }
        }
        return (int)Math.floor(((double) sum / arr.size()));
    }
}
