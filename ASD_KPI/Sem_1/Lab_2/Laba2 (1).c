#include <stdio.h>
#include <math.h>

int main(){
 
 int n;
 int amount = 0;
 double res = 0;
 double power = 1;
 
 printf("Enter n: ");
 scanf("%d", &n);
 for (int i = 1; i <= n; i++) {
   power *= 2;
   double x = ((power + 1 ) * (power + 1 ));
   double counter = 1;
   for (int j = 1; j <= i; j++) {
     counter *= (j + 1);
     amount += 2;
   }
   res += counter / x   ;
   amount += 5;
 }
  
  printf("Amount of calls: ");
 printf("%d", amount);
 printf("\nResult: ");
 printf("%.7f", res);
return 0; 
}
