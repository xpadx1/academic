#include <stdio.h>
#include <stdlib.h>
#include <windows.h>

#define COLUMN 80
#define ROW 24

#define SLEEP_TIME 10

void gotoXY(int x, int y) {
    COORD coord;
    coord.X = y;
    coord.Y = x;
    SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), coord);
}

int main() {

    int n = COLUMN;
    int m = ROW;
    int p;
    int i, j;
    int matrix[ROW][COLUMN] = { 0 };

    for (p = m / 2; p > 0; p--) {

        for (j = p; j <= n - p; j++) {

            i = p - 1;
            gotoXY(i, j);
            printf("%d", matrix[i][j]);
            Sleep(SLEEP_TIME);
        }

        for (i = p - 1; i < m - p + 1; i++) {

            j = n - p;
            gotoXY(i, j);
            printf("%d", matrix[i][j]);
            Sleep(SLEEP_TIME);
        }


        for (j = n - 1 - p; j >= p - 1; j--) {

            i = m - p;
            gotoXY(i, j);
            printf("%d", matrix[i][j]);
            Sleep(SLEEP_TIME);
        }


        for (i = m - p; i >= p - 1; i--) {

            j = p - 1;
            gotoXY(i, j);
            printf("%d", matrix[i][j]);
            Sleep(SLEEP_TIME);
        }
    }

    gotoXY(m, 0);
    return 0;
}
