# Mastermind



## Objective

This is an implementation of the game Mastermind using C#.



## Game Rules

- The code is made of 4 distinct digits from 0 to 8.

- You have a limited number of attempts (default: 10).

- Each round, you get feedback:

- Well placed pieces: correct digit in the correct position.

- Misplaced pieces: correct digit but wrong position.



## Example

> ./dotnet run my_mastermind -- -c 0123 -t 10

Can you break the code? Enter a valid guess.

---Round 0>1456

Well placed pieces: 0

Misplaced pieces: 1

---Round 1>0123

Congratz! You did it!



