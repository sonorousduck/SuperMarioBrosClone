extends Node

class_name GameManager

var best_score = 0
var current_score = 0

var time_remaining = 400
var coins = 0
var lives = 10

signal score_updated(score: int)

func add_points(point_amount: int):
	current_score += point_amount
	
	score_updated.emit(current_score)
	
	
func add_coins():
	coins += 1

func game_over():
	coins = 0
	if (best_score < current_score):
		best_score = current_score
	current_score = 0
