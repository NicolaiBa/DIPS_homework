clear
storage=$(mcs -out:Bank.exe Bank.cs)
echo $storage > test.txt
echo " "
./Bank.exe
