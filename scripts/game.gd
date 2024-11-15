extends Node2D
class_name Game


@export var ui: UI
@export var game_manager: GameManager

func _ready():
	if (!game_manager.score_updated.is_connected(ui._update_points)):
		game_manager.score_updated.connect(ui._update_points)
	if (!game_manager.coin_updated.is_connected(ui._update_coins)):
		game_manager.coin_updated.connect(ui._update_coins)
	if (!game_manager.lives_updated.is_connected(ui._update_lives)):
		game_manager.lives_updated.connect(ui._update_lives)
	if (!game_manager.time_updated.is_connected(ui._update_time)):
		game_manager.time_updated.connect(ui._update_time)
