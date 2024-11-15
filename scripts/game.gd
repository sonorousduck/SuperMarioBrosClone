extends Node2D
class_name Game


@export var ui: UI
@export var game_manager: GameManager


func _ready():
	if (!game_manager.score_updated.is_connected(ui._update_points)):
		game_manager.score_updated.connect(ui._update_points)
