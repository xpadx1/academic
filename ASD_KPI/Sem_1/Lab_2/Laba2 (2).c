#include <stdio.h>
#include <math.h>
int main(){
 int n;
 printf("Enter n: ");
 scanf("%d", &n);
 double res = 0;
 double counter1 = 0;
 double power = 1;
 double counter = 1;
 int amount = 0;
 for (int i = 1; i <= n; i++) {
   power *= 2;
   counter *= (i + 1);
   counter1 = ((power + 1 ) * (power + 1 ));
   res += counter /  counter1 ;
   amount += 7;
 }
 printf("Amount of calls: ");
 printf("%d", amount);
 printf("\nResult: ");
 printf("%lf", res);
 return 0;
}
