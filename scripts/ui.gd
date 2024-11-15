extends CanvasLayer

class_name UI
@onready var score_label: Label = %Score


var score = 0:
	set(new_score):
		score = new_score
		_update_score_label()

func _update_score_label():
	score_label.text = str(score)


func _ready() -> void:
	_update_score_label()
	
	

func _update_points(point_value: int) -> void:
	score = point_value
