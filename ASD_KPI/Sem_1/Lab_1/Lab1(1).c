#include <stdio.h>
int main() {
	float x;
	printf("x:");
	scanf("%f", &x);
	if ((x > -10 && x <= -5) || (x > 5 && x <= 15)) {
		printf("%f", 3 * x - 6);
	}
	else if (x > 25) {
		printf("%f", 6 * x - 3 * x + 2);
	}
	else {
		printf("%s", "function does not exist");
	}
	return 0;
}