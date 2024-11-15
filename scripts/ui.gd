extends CanvasLayer

class_name UI
@onready var score_label: Label = %Score
@onready var coins_label: Label = %Coins
@onready var world_label: Label = %World
@onready var time_label: Label = %Time
@onready var lives_label: Label = %Lives


var score = 0:
	set(new_score):
		score = new_score
		_update_score_label()

var coins = 0:
	set(new_coins):
		coins = new_coins
		_update_coin_label()

var time = 400:
	set(new_time):
		time = new_time
		_update_time_label()


var lives = 3:
	set(new_lives):
		lives = new_lives
		_update_life_label()

func _update_score_label():
	score_label.text = str(score)
	
func _update_coin_label():
	coins_label.text = str(coins)

func _update_time_label():
	time_label.text = str(time)

func _update_life_label():
	lives_label.text = str(lives)


func _ready() -> void:
	_update_score_label()
	_update_coin_label()
	_update_time_label()
	_update_life_label()
	
	

func _update_points(point_value: int) -> void:
	score = point_value
func _update_lives(new_lives: int) -> void:
	lives = new_lives
func _update_coins(new_coins: int) -> void:
	coins = new_coins
func _update_time(new_time: int) -> void:
	time = new_time
