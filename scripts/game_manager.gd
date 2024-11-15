extends Node

class_name GameManager

var best_score := 0
var current_score := 0

var time_remaining := 400
var coins := 0
var lives := 10

@onready var music: AudioStreamPlayer = %Music
signal score_updated(score: int)
signal coin_updated(coins: int)
signal lives_updated(lives: int)
signal time_updated(time: int)

# Timestamps of frames rendered in the last second
var times := []

# Frames per second
var fps := 0

var seconds: float = 0.0

func add_points(point_amount: int):
	current_score += point_amount
	score_updated.emit(current_score)

func _process(delta: float) -> void:
	#var now := Time.get_ticks_msec()
#
	## Remove frames older than 1 second in the `times` array
	#while times.size() > 0 and times[0] <= now - 1000:
		#times.pop_front()
#
	#times.append(now)
	#fps = times.size()
#
	## Display FPS in the label
	#print(str(fps) + " FPS")
	
	seconds += delta
	if (seconds >= 1.0):
		seconds -= 1
		subtract_time()
	
	
func add_coins():
	coins += 1
	coin_updated.emit(coins)

func add_life():
	lives += 1
	lives_updated.emit(lives)

func lose_life():
	lives -= 1
	lives_updated.emit(lives)
	
	
func subtract_time():
	time_remaining -= 1
	time_updated.emit(time_remaining)

func game_over():
	coins = 0
	if (best_score < current_score):
		best_score = current_score
	current_score = 0
	time_remaining = 400	
	lose_life()
