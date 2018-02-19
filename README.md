# Smart-Ball

Smart Ball is a project I created in Unity that uses a neural network and a genetic algorithm to teach balls to safetly navigate a course. 

<a href="https://imgflip.com/gif/24u5nl"><img src="https://i.imgflip.com/24u5nl.gif" title="made at imgflip.com"/></a>

# The Brain (Neural Network)

The program starts by initializing x number of training balls. Each one of these training balls is assigned a random DNA sequence that is used for the weights of the connections in its neural network (Its brain). The neural network takes 5 distances as inputs by using raycasts that are sent from the ball it is assigned to. The distance is the minimum of the closest object hit and a user specified distance. These distances come from the left, forward-left, forward, forward-right, and right (picture below). Every frame the neural network of each ball will make a decision on how much the ball should rotate based on these 5 inputs. If the ball hits a wall, it will die.

<img width="250" alt="screen shot 2018-02-19 at 2 55 08 am" src="https://user-images.githubusercontent.com/12948431/36367561-3ae77b60-1521-11e8-81b9-09ff41ef7897.png">

# Genetic Algorithm

The genetic algorithm used consisted of uniform DNA crossover, roullette selection, elitism and random mutation.

# Fitness

To score a ball (and its brain's performance) a fitness score was used that increased every frame a ball was alive and was higher based on its distance from the walls around it.
